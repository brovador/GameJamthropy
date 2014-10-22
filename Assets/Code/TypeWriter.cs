using UnityEngine;
using System.Collections;

public class TypeWriter : MonoBehaviour {

	public delegate void HandleInputType();
	public event HandleInputType OnInputRight;
	public event HandleInputType OnInputWrong;
	
	public TextAsset 	textAsset;
	public UILabel 		lblBackText, lblFrontText, lblCurrentChar, lblNextText, lblBlinkChar;

	public AudioClip	typeWrong;
	public AudioClip	typeRight;

	private string 		originalText;
	private string 		charToWrite;
	private string 		auxWord;
	private string 		originalPhrase ;
	private string[] 	aLine;
	private int 		countLine;

	void Start () 
	{
		countLine 			= 0;
		originalText		= textAsset.text;
		aLine				= originalText.Split('\n');
		lblBackText.supportEncoding = true;
		lblFrontText.supportEncoding = true;
		GivePhrase(aLine, countLine);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.RightShift) && !Input.GetKeyDown(KeyCode.RightAlt) && !Input.GetKeyDown(KeyCode.LeftAlt))
		{
			//When you type a good char
			if(Input.inputString.ToLower() == charToWrite.ToLower())
			{
				SoundManager.Instance.Play2DSound(typeRight);

				//Put the char in the front text
				lblFrontText.text += charToWrite;

				//Debug.Log(auxWord.Length);
				//When you finish the word correctly
				if(auxWord.Length == 1)
				{
					countLine++;
					GivePhrase(aLine, countLine);
					lblBlinkChar.text = lblBlinkChar.text.Trim();
				}
				else
				{
					//Delete the first char
					auxWord = RemoveFirstChar(auxWord);
					//Get the next char
					charToWrite = GetFirstChar(auxWord);

					while(charToWrite == "\t")
					{
						auxWord = RemoveFirstChar(auxWord);
						charToWrite = GetFirstChar(auxWord);
					}

					lblBlinkChar.text = lblBlinkChar.text.Insert(0, " ");
					if(OnInputRight != null)
						OnInputRight();
					lblFrontText.color = Color.green;
				}
			}
			else
			{
				SoundManager.Instance.Play2DSound(typeWrong);

				if(OnInputRight != null)
					OnInputWrong();
				lblFrontText.color = Color.red;	
			}
		}
	}



	private string GetFirstChar(string _text)
	{	
		lblCurrentChar.text = _text.Substring(0, 1);
		if(_text.Substring(0, 1) == " ")
			lblCurrentChar.text = "Space";

		return _text.Substring(0, 1);
	}

	private string RemoveFirstChar(string _text)
	{
		return _text.Substring(1, _text.Length - 1);
	}

	private void GivePhrase(string[] _text, int _line)
	{
		originalPhrase		= _text[_line];
		lblNextText.text		= _text[_line + 1];

		if(string.IsNullOrEmpty(_text[_line]))
		{
			countLine++;
			GivePhrase(_text, countLine);
		}
		else if(string.IsNullOrEmpty(_text[_line + 1]))
		{
			lblNextText.text		= _text[_line + 2];
		}


		lblBackText.text 		= originalPhrase;

		lblFrontText.text		= "";
		auxWord				= originalPhrase; 
		charToWrite 		= GetFirstChar(originalPhrase);
	}
}
