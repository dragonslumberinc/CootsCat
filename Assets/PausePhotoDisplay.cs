using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePhotoDisplay : MonoBehaviour
{
    public Photo[] photos;

    public void setPhotos()
    {
        Texture texture;
        string title;
        int active = 0;
        if (GameController.Instance != null)
        {
            do
            {
                texture = GameController.Instance.pictureTaker.getLastTexture(active);
                title = GameController.Instance.pictureTaker.getLastTitle(active);

                if (texture != null)
                {
                    photos[active].gameObject.SetActive(true);
                    photos[active].showPhoto(texture, title);
                }
                else
                {
                    photos[active].gameObject.SetActive(false);
                }

                active++;
            } while (photos.Length > active);
        }
    }
}
