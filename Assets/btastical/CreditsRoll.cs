using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsRoll : MonoBehaviour
{
    private List<Contributor> contributors;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;

    protected void Awake()
    {
        contributors = new List<Contributor>();
        this.contributors.Add(new Contributor("Potion Pong", string.Empty, 130, 0, 5f));
        this.contributors.Add(new Contributor("HardlyDifficult", "Code, Management & wizardry", 90, 50, 3f));

        TextAsset text = Resources.Load<TextAsset>("contributions");

        string[] lines = text.text.Split(
            new[] { '\n' },
            StringSplitOptions.None
        );

        bool firstLine = true;

        foreach (var splittedLine in lines)
        {
            if (firstLine)
            {
                firstLine = false;
                continue;
            }

            string[] splitLine = splittedLine.Split(',');
            contributors.Add(new Contributor(splitLine[1], splitLine[2], 90, 50, 2f));
        }
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(RollCredits());
    }

    private GameObject CreateContributorGameObject(Contributor contributor)
    {
        GameObject parentGameObject = GameObject.Find("Textroll");
        GameObject contributorGameObject = new GameObject("Credit-" + contributor.Name);
        GameObject responsibleForGameObject = new GameObject("ResponsibleFor-" + contributor.Name);
        contributorGameObject.transform.SetParent(parentGameObject.transform);
        responsibleForGameObject.transform.SetParent(contributorGameObject.transform);

        Text contributorText = contributorGameObject.AddComponent<Text>();
        contributorText.text = contributor.Name;
        contributorText.alignment = TextAnchor.MiddleCenter;
        contributorText.color = new Color(.3f, .3f, .3f);
        contributorText.fontSize = contributor.NameSize;
        contributorText.font = Resources.Load<Font>("wizzta");
        contributorText.rectTransform.localPosition = new Vector3(10, -155, 0);
        contributorText.horizontalOverflow = HorizontalWrapMode.Overflow;
        contributorText.verticalOverflow = VerticalWrapMode.Overflow;

        Text responsibleForText = responsibleForGameObject.AddComponent<Text>();
        responsibleForText.text = contributor.ResponsibleFor;
        responsibleForText.alignment = TextAnchor.MiddleCenter;
        responsibleForText.fontSize = contributor.ResponsibleSize;
        responsibleForText.color = Color.white;
        responsibleForText.font = Resources.Load<Font>("wizzta");
        responsibleForText.rectTransform.localPosition = new Vector3(10, -70, 0);
        responsibleForText.horizontalOverflow = HorizontalWrapMode.Overflow;
        responsibleForText.verticalOverflow = VerticalWrapMode.Overflow;

        return contributorGameObject;
    }

    private IEnumerator RollCredits()
    {
        foreach (var contributor in this.contributors)
        {
            StartCoroutine(RollOne(this.CreateContributorGameObject(contributor)));
            yield return new WaitForSecondsRealtime(contributor.WaitBeforeNextInSeconds);
        }
    }

    private IEnumerator RollOne(GameObject contributorGameObject)
    {
        Text textObject = contributorGameObject.GetComponent<Text>();
        while (textObject.rectTransform.localPosition.y < 350)
        {
            float newHeight = textObject.rectTransform.localPosition.y + 10;
            Vector3 targetPosition = new Vector3(textObject.rectTransform.localPosition.x, newHeight, 0);

            textObject.rectTransform.localPosition = 
                Vector3.SmoothDamp(textObject.rectTransform.localPosition, 
                    targetPosition, 
                    ref velocity, 
                    smoothTime);

            yield return new WaitForFixedUpdate();
        }

        Destroy(contributorGameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}

public class Contributor
{
    public string Name { get; set; }
    public string ResponsibleFor { get; set; }
    public int NameSize { get; set; }
    public int ResponsibleSize { get; set; }
    public float WaitBeforeNextInSeconds { get; set; }

    public Contributor(string contributor, 
        string responsibleFor, 
        int nameSize, 
        int responsibleSize, 
        float waitBeforeNextInSeconds)
    {
        this.NameSize = nameSize;
        this.ResponsibleSize = responsibleSize;
        this.WaitBeforeNextInSeconds = waitBeforeNextInSeconds;
        this.Name = this.Clean(contributor);
        this.ResponsibleFor = this.Clean(responsibleFor);
    }

    private string Clean(string uncleaned)
    {
        return uncleaned.Replace("\"", "");
    }
}