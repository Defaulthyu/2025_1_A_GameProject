using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI ��� - Inspector ���� ����")]
    public GameObject DialoguePanel;                //��ȭâ ��ü �г�
    public Image characterImage;                    //ĳ���� �̹���
    public TextMeshProUGUI characternameText;       //ĳ���� �̸�
    public TextMeshProUGUI dialogueText;            //��ȭ ���� ǥ�� �ؽ�Ʈ
    public Button nextButton;                       //���� ��ư

    [Header("�⺻ ����")]
    public Sprite defaultcharacterImage;            //ĳ���� �⺻ �̹���

    [Header("Ÿ���� ȿ�� ����")]
    public float typingSpeed = 0.05f;               //���� �ϳ��� ��� �ӵ�
    public bool skipTypingOnClick = true;           //Ŭ�� �� Ÿ���� ��� �Ϸ� ����

    //���� ������
    private DialogueDataSO currentDialogue;          //���� �������� ��ȭ ������
    private int currentLineIndex = 0;                //���� �� ��° ��ȭ ������ (0���� ����)
    private bool isDialogueActive = false;           //��ȭ ���������� Ȯ�� �ϴ� �÷���
    private bool isTyping = false;                   //���� Ÿ���� ȿ���� ���� ������ Ȯ��
    private Coroutine typingCoroutine;               //Ÿ���� ȿ�� �ڷ�ƾ ���� (������)

    // Start is called before the first frame update
    void Start()
    {
        DialoguePanel.SetActive(false);             //��ȭ â �����
        nextButton.onClick.AddListener(HandleNextInput);    //���� ��ư�� ���ο� �Է� ó�� ����
    }

    // Update is called once per frame
    void Update()
    {
        if(isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();                      //���� �Է� ó��(Ÿ���� ���̸� �Ϸ�, �ƴϸ� ���� ��)
        }
    }

    IEnumerator TypeText(string textToType)         //Ÿ���� �� ��ü �ؽ�Ʈ
    {
        isTyping = false;                           //Ÿ���� ����
        dialogueText.text = "";                     //�ؽ�Ʈ �ʱ�ȭ

        for(int i = 0; i< textToType.Length; i++)   //�ؽ�Ʈ�� �� ���ھ� �߰�
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);       //��� �ð� ����
        }

        isTyping = false;                           //Ÿ���� �Ϸ�
    }

    private void CompleteTyping()                   //Ÿ���� ȿ���� ��� �Ϸ��ϴ� �Լ�
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);         //�ڷ�ƾ ����
        }
        isTyping = false;                       //Ÿ���� ���� ����

        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];        //���� ���� ��ü �ؽ�Ʈ�� ��� ǥ��

        }
    }

    void ShowCurrentLine()          //���� ��ȭ ���� ������ Ÿ���� ȿ���� �Բ� ȭ�鿡 ǥ���ϴ� �Լ�
    {
        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)   //��ȭ �ε����� ��ȿ���� Ȯ��
        {
            if(typingCoroutine != null)         //���� Ÿ���� ȿ���� �ִٸ� ����
            {
                StopCoroutine(typingCoroutine); //�ڷ�ƾ ����
            }
        }
        //���� ���� ��ȭ �������� Ÿ���� ȿ�� ����
        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    void EndDialogue()          //��ȭ�� ������ �����ϴ� �Լ�
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);     //�ڷ�ƾ ����
            typingCoroutine = null;

        }

        isDialogueActive = false;           //��ȭâ ��Ȱ��ȭ
        isTyping = false;                   //��ȭâ ���� ����
        DialoguePanel.SetActive(false);     //��ȭâ �����
        currentLineIndex = 0;               //�ε��� �ʱ�ȭ
    }

    public void ShowNextLine()              //���� ��ȭ ��� �̵���Ű�� �Լ� (Ÿ������ �Ϸ�� �Ŀ��� ȣ��
    {
        currentLineIndex++;                 //���� �ٷ� �ε��� ����

        if(currentLineIndex >= currentDialogue.dialogueLines.Count) //������ ��ȭ������ Ȯ��
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();                          //��ȭ�� ���������� ���� �� ǥ��
        }
    }

    public void HandleNextInput()                       //�����̽��ٳ� ��ư Ŭ�� �� ȣ�� �Ǵ� �Է� ó�� �Լ�
    {
        if(isTyping && skipTypingOnClick)
        {
            CompleteTyping();                           //Ÿ���� ���̶�� ��� �Ϸ�
        }
        else if (!isTyping)
        {
            ShowNextLine();                             //Ÿ���� �Ϸ� ���¸� ���� �� ǥ��
        }
    }

    public void SkipDialogue()                          //��ȭ ��ü�� �ٷ� ��ŵ�ϴ� �Լ�
    {
        EndDialogue();
    }

    public bool IsDialogueActive()                      //���� ��ȭ�� ���� ������ Ȯ�� �ϴ� �Լ�
    {
        return isDialogueActive;
    }

    public void StartDialogue(DialogueDataSO dialogue)  //���ο� ��ȭ�� ���� �ϴ� �Լ�
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;      //��ȭ �����Ͱ� ���ų� ��ȭ ������ ��������� ���� ���� ����

        //��ȭ ���� �غ�
        currentDialogue = dialogue;             //���� ��ȭ ������ ����
        currentLineIndex = 0;                   //ù ��° ��ȭ ���� ����
        isDialogueActive = true;                //��ȭ Ȱ��ȭ �÷��� ��

        //UI ������Ʈ
        DialoguePanel.SetActive(true);                   //��ȭâ ���̱�
        characternameText.text = dialogue.characterName; //ĳ���� �̸� ǥ��

        if(characterImage != null)
        {
            if(dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;        //��ȭ �������� �̹��� ���
            }
            else
            {
                characterImage.sprite = defaultcharacterImage;          //�⺻ �̹��� ���
            }
        }

        ShowCurrentLine();                      //ù ��° ��ȭ ���� ǥ��
    }
}
