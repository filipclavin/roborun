using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private float shownTimer;
    private Shader shader;
    private TMP_Text scoreText;

    [SerializeField] private float shownDuration;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        shader = GetComponent<MeshRenderer>().material.shader;
    }

    public void ScoreTextUpdate(float shownScore)
    {
        scoreText.text = shownScore.ToString();
    }

    private void Update()
    {
        shownTimer += Time.deltaTime;
        //shader.a -= ((256 / shownDuration) * Time.deltaTime);
        //= new Color(textColor.r, textColor.g, textColor.b, textColor.a - ((256 / shownDuration) * Time.deltaTime));
        if (shownTimer > shownDuration) 
        {
            Destroy(gameObject);
        }
    }
}
