using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Text.RegularExpressions;
using KoreanTyper;

public class DialogueRunner : MonoBehaviour, IDataInitializable
{
    [SerializeField] private DialogueParser parser;
    public TextAsset DialogueFile;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI DialogueText;

    [Header("DialogueBox 관련 요소들")]
    [SerializeField] private GameObject DialoguePanel; //대화창 전체 부모 오브젝트.
    [SerializeField] private GameObject ChoiceButtonPrefab; //선택지 버튼 프리팹
    [SerializeField] private Transform OptionContainer; // ChoiceButton들의 부모 오브젝트

    private List<DialogueParser.ParsedLine> scriptLine;
    private int currentLineNum;

    void Start()
    {

    }

    public void DataInitialize()
    {
        currentLineNum = 0;
    }

    public void Parsing()
    {
        scriptLine = parser.Parse(DialogueFile.ToString());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProccessNextLine();
            //LocalGameManager.instance.DisableAllInput();
        }
    }

    public void DialogueBoxGlitch()
    {
        DialoguePanel.transform.GetChild(0).GetComponent<DialogueBoxGlitch>().StartGlitch();
    }

    public void StopDialogueBoxGlitch()
    {
        var glitchComponent = DialoguePanel.transform.GetChild(0).GetComponent<DialogueBoxGlitch>();
        if (glitchComponent != null)
        {
            glitchComponent.StopGlitch();
        }
        SoundManager.instance.PlayBgm("기본 bgm");
    }

    public void EndDialogue()
    {
        StopDialogueBoxGlitch(); // 글리치 효과 및 BGM 원상복구
        currentLineNum = 0;
        DialoguePanel.SetActive(false);
        LocalGameManager.instance.EnableAllInput();
        LocalGameManager.instance.unitManager.SummonEnemy(LocalGameManager.instance.gameProccessManager.proccess);
        if (LocalGameManager.instance.timeLineManager.timeLinePlaying)
        {
            PlayTimeLine();
        }
    }

    public void StopAndReset()
    {
        StopAllCoroutines(); // 진행중인 타이핑 코루틴 중지
        currentLineNum = 0;
        DialoguePanel.SetActive(false);
        // EndDialogue와 달리, 입력 활성화나 적 소환은 하지 않음
    }

    public void StartDialogue()
    {
        Parsing();
        DialoguePanel.SetActive(true);
        LocalGameManager.instance.DisableAllInput();
        ProccessNextLine();
    }

    public void PauseTimeLine()
    {
        LocalGameManager.instance.timeLineManager.director.Pause();
    }

    public void PlayTimeLine()
    {
        LocalGameManager.instance.timeLineManager.director.Play();
    }

    public void ProccessNextLine()
    {
        if (currentLineNum >= scriptLine.Count) //현재 라인 넘버가 실행중인 대화 스크립트의 총 줄 개수보다 많거나 같으면.
        {
            EndDialogue(); //대화 종료.
            return;
        }
        DialogueParser.ParsedLine line = scriptLine[currentLineNum];

        switch (line.Action)
        {

            case "T": //액션 노드가 T일 경우, 대사를 출력.
                {
                    RunningDialogue(line);
                    break;
                }

            case "S": //액션 노드가 S일 경우, 선택지를 제시한 후, 고른 선택지에 따라 줄 이동.
                {
                    string condition = line.Detail.Split('_')[0];
                    string result = line.Detail.Split('_')[1];
                    HandleChoices(condition.Split('|'), result.Split('|'), line);
                    return;
                }

            case "J":
                {
                    int jumpLine = int.Parse(line.Detail);
                    currentLineNum = currentLineNum + jumpLine;
                    ProccessNextLine();
                    return;
                }

            case "": //액션 노드에 아무것도 없는 경우. T행동의 연장선으로 간주, Detail의 Result를 출력한다. T행동의 연장이 아니더라도, 다른 노드를 실행.
                {
                    RunningDialogue(line);
                    break;
                }

            default:
                {
                    Debug.LogWarning($"알 수 없는 액션: {line.Action} (라인 {currentLineNum})");
                    currentLineNum++;
                    ProccessNextLine();
                    return;
                }
        }

        RunningOtherNode(line);
        currentLineNum++;
    }

    private void RunningOtherNode(DialogueParser.ParsedLine line)
    {
        if (line.BGM != "") LocalGameManager.instance.dialogueFuncManager.ChangeBGM(line.BGM); //브금 변경.
        if (line.Result != "") LocalGameManager.instance.dialogueFuncManager.Learning(line.Result);
    }

    private void HandleChoices(string[] selectors, string[] results, DialogueParser.ParsedLine line)
    {
        for (int i = 0; i < selectors.Length; i++)
        {
            selectors[i] = selectors[i].Trim();
            results[i] = results[i].Trim();
        }

        //DialoguePanel.SetActive(false);

        for (int i = 0; i < selectors.Length; i++)
        {
            var buttonObj = Instantiate(ChoiceButtonPrefab, OptionContainer);

            var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            var button = buttonObj.GetComponent<Button>();
            int index = i;

            buttonText.text = selectors[i];
            button.onClick.AddListener(() => OptionSelected(int.Parse(results[index]), line));
        }
    }

    private void OptionSelected(int lineIndex, DialogueParser.ParsedLine line)
    {
        //ChoiceOptionPanel.SetActive(false);
        DialoguePanel.SetActive(true);
        //currentLineNum = lineIndex;

        foreach (Transform child in OptionContainer)
        {
            Destroy(child.gameObject);
        }

        Debug.Log($"줄 이동! 이전 줄 : {currentLineNum}, 뛰어넘을 줄{lineIndex + 1}");
        currentLineNum += lineIndex + 1;
        ProccessNextLine();
    }

    private void RunningDialogue(DialogueParser.ParsedLine line)
    {
        //Debug.Log(line.Detail.condition);
        var shakeEffect = DialogueText.GetComponent<DialogueTextManager>();
        shakeEffect.shakeRanges.Clear();

        // 딜레이 태그를 제외한 텍스트에서 흔들림 범위를 계산해야 인덱스가 맞음
        string delayPattern = @"\((\d+(\.\d+)?)\)";
        string textWithoutDelays = Regex.Replace(line.Detail, delayPattern, "");

        var matches = Regex.Matches(textWithoutDelays, @"\{(.*?)\}");
        int tagOffset = 0;

        foreach (Match match in matches)
        {
            string innerText = match.Groups[1].Value;

            int startIdx = match.Index - tagOffset;
            int endIdx = startIdx + innerText.Length - 1;

            shakeEffect.shakeRanges.Add((startIdx, endIdx));

            tagOffset += 2;
        }

        //대사에 \n이 포함되어 있을 경우, 줄바꿈 처리.
        if (line.Detail.Contains("\\n")) line.Detail = line.Detail.Replace("\\n", "\n");

        //대사에 '{}'가 존재할 경우, 임시 문자로 변경.
        line.Detail = Regex.Replace(line.Detail, @"\{(.*?)\}", "⦃$1⦄");

        /*Debug.Log(int.Parse(line.Actor.Split('_')[0]));*/

        StartCoroutine(TypingTxt(line.Detail)); //대사 출력.
        //GameManager.instance.dataManager.dialogueLog.Add(line); //대화 로그에 저장.
    }

    private IEnumerator TypingTxt(string args)
    {
        // 진단용 코드: KoreanTyper를 사용하지 않고, 딜레이 태그도 무시합니다.
        string fullText = args; // ⦃ ⦄ 마커가 포함된 텍스트

        // 이전 타이핑이 남아있지 않도록 초기화
        DialogueText.text = "";
        yield return null;

        for (int i = 1; i <= fullText.Length; i++)
        {
            string partialText = fullText.Substring(0, i);
            DialogueText.text = partialText.Replace("⦃", "<color=#FF0000>").Replace("⦄", "</color>");
            yield return new WaitForSeconds(0.05f);
        }

        // 최종 텍스트를 완전한 태그로 설정
        DialogueText.text = fullText.Replace("⦃", "<color=#FF0000>").Replace("⦄", "</color>");
        yield return null;
    }
}
