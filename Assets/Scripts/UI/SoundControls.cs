using UnityEngine;
using System.Collections;

public class SoundControls : MonoBehaviour {

	public void ToggleMusic()
    {
        // alternar música / som somente quando o usuário clicar no botão som / música,        
        // então esperamos um tempo para evitar chamar a função de alternância quando a definimos manualmente a partir de PlayerPrefs        
        if (Time.time > .3f)     
		    SoundManager.Instance.ToggleMuteMusic();
	}

	public void ToggleSoundEffect()
    {
        if (Time.time > .3f)
		    SoundManager.Instance.ToggleMuteSoundEffects();
	}
}
