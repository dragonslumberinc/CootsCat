using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureTaker : MonoBehaviour
{
    public Camera camera;

    public RenderTexture[] renderTextures;

    public string[] titles;

    public PhotoDisplay photoDisplay;

    private int renderPos = 0;

    private bool canTakePhoto = true;

    void Awake()
    {
        init();
    }

    public void init()
    {
        renderPos = 0;
        titles = new string[renderTextures.Length];
    }

    public void takePicture(Transform source, Transform target, string title, float delay = 0.1f)
    {
        if (canTakePhoto)
        {
            if (renderPos < renderTextures.Length)
            {
                StopAllCoroutines();
                StartCoroutine(takePictureRoutine(source, target, title, delay));
            }
        }
        else
        {
            GameController.Instance.backOneFireOffset();
        }
    }

    public IEnumerator takePictureRoutine(Transform source, Transform target, string title, float delay)
    {
        yield return new WaitForSeconds(delay);
        canTakePhoto = false;

        Vector3 posMiddle = (source.position + target.position) / 2f;
        Vector3 posLookAt = posMiddle;
        Vector3 posCamera = (target.position - source.position).normalized;
        float distance = Vector3.Distance(source.position, target.position);

        distance /= 1.5f;
        posCamera = Quaternion.AngleAxis(Random.Range(-60, -120), Vector3.up) * posCamera;
        RaycastHit hit;
        Physics.Raycast(posMiddle, posCamera, out hit, 1000f);

        if (distance > hit.distance - 0.5f)
        {
            distance = hit.distance - 0.5f;
            posLookAt = Random.Range(0, 2) == 0 ? source.position : target.position;
        }
        posCamera = posMiddle + (posCamera * distance);
        posCamera.y = posMiddle.y + 0.5f;

        transform.position = posCamera;
        transform.LookAt(posLookAt);
        camera.targetTexture = renderTextures[renderPos];
        titles[renderPos] = title;
        renderPos++;

        camera.enabled = true;
        yield return new WaitForEndOfFrame();
        camera.enabled = false;

        yield return new WaitForEndOfFrame();
        photoDisplay.showPhoto(getLastTexture(), getLastTitle());

        yield return new WaitForSeconds(5.5f);
        canTakePhoto = true;
    }

    public Texture getLastTexture(int offset = 0)
    {
        if(renderPos > offset && renderTextures.Length > (renderPos - 1 - offset))
            return renderTextures[renderPos - 1 - offset];
        return null;
    }

    public string getLastTitle(int offset = 0)
    {
        if (renderPos > offset && titles.Length > (renderPos - 1 - offset))
            return titles[renderPos - 1 - offset];
        return null;
    }
}
