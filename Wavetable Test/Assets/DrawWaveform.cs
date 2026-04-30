using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawWaveform : MonoBehaviour
{
    static int wavetable_length = 512;
    float[] wavetable = new float[wavetable_length];
    Transform canvasTransform;
    public TMP_Text debugText;
    [SerializeField] private LineRenderer line;

    [Range(0.1f, 0.5f)]
    public float lineThickness;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasTransform = GetComponent<Transform>();
        //fill wavetable with 0s to start
        for(int i =0; i < wavetable_length; i++)
        {
            wavetable[i] = 0f;
        }
        //configure number of "segments" and thickness
        line.positionCount = wavetable_length;
        line.startWidth = lineThickness;
        line.endWidth = lineThickness;
        //draw line
        UpdateLine();
    }


    // Update is called once per frame
    void Update()
    {
        //if mouse1 is held down (would be controller's trigger in VR)
        if (Mouse.current.leftButton.isPressed)
        {
            //get mouse position (would be ray interactor in VR)
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //canvas hit detected
                if (hit.transform == canvasTransform)
                {
                    Vector3 hitPoint = hit.point;

                    //get vector of hit ray relative to the canvas
                    Vector2 uv = hit.textureCoord;

                    //map to wavetable
                    int index = (int)(uv.x * 511);
                    wavetable[index] = uv.y * 2f - 1f;
                    UpdateLine();
                    if (debugText)
                    {
                        debugText.SetText($"Hit point to wavetable index: {index} \n index amplitude :{wavetable[index]}");
                    }
                }
            }
        }
    }

    void UpdateLine()
    {
        Renderer r = GetComponent<Renderer>();
        Bounds b = r.bounds;
        for (int i = 0; i < wavetable_length; i++)
        {
            float x = Mathf.Lerp(-0.5f, 0.5f, i / (float)(wavetable_length - 1));
            float y = wavetable[i] * 0.5f;

            line.SetPosition(i, new Vector3(x, y, b.center.z+0.001f));
        }
    }



    public float[] GetWavetable()
    {
        return wavetable;
    }
}
