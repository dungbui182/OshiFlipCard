using GrandDreams.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Components
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class BitmapText : MonoBehaviour
    {

        #region Declare Variables

        [System.Serializable]
        public class BitmapCharacter
        {
            public GameObject GameObject;
            public RectTransform RectTransform;
            public Image Image;

            public BitmapCharacter(string name)
            {
                GameObject = new GameObject(name);
                RectTransform = GameObject.AddComponent<RectTransform>();
                Image = GameObject.AddComponent<Image>();

                RectTransform.anchoredPosition3D = Vector3.zero;
            }

            public BitmapCharacter(string name, Transform tfParent)
            {
                GameObject = new GameObject(name);
                RectTransform = GameObject.AddComponent<RectTransform>();
                Image = GameObject.AddComponent<Image>();

                RectTransform.anchoredPosition3D = Vector3.zero;

                RectTransform.SetParent(tfParent);
                RectTransform.localScale = Vector3.one;
                RectTransform.localEulerAngles = Vector3.zero;

                GameObject.SetActive(false);
            }
        }

        [SerializeField] private string text = "";
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;

                if(!isInitialized)
                {
                    return;
                }

                char[] chars = text.ToCharArray();
                int textLength = chars.Length > length ? length : chars.Length;

                for (int i = 0; i < textLength; i++)
                {
                    bitmapCharacters[i].GameObject.SetActive(true);
                    bitmapCharacters[i].Image.sprite = dictionaryChar[chars[i]];
                }

                for (int i = textLength; i < length; i++)
                {
                    bitmapCharacters[i].GameObject.SetActive(false);
                }
            }
        }

        [SerializeField] private Sprite[] spriteCharacters;
        [SerializeField] private char[] characters;

        [SerializeField] private int length = 1;
        [SerializeField] private int maxCharacterInLine = 1;
        [SerializeField] private Vector2 spacing = Vector2.zero;
        [SerializeField] private Vector2 characterSize = Vector2.zero;

        [SerializeField] private TextAnchor characterAlignment = TextAnchor.UpperCenter;

        private GridLayoutGroup layoutGroup;

        private Dictionary<char, Sprite> dictionaryChar = new Dictionary<char, Sprite>();
        private List<BitmapCharacter> bitmapCharacters;

        private bool isInitialized = false;

        #endregion Declare Variables

        private void Awake()
        {
            layoutGroup = gameObject.AddComponentIfNotExist<GridLayoutGroup>();

            layoutGroup.cellSize = characterSize;
            layoutGroup.spacing = spacing;
            layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layoutGroup.constraintCount = maxCharacterInLine;
            layoutGroup.childAlignment = characterAlignment;

            InstantiateImageCharacters(length);

            isInitialized = MapCharacterAndSprite();
        }

        private void OnDestroy()
        {
            
        }

        

        #region Public Function



        #endregion Public Function

        #region Private Function

        private bool MapCharacterAndSprite()
        {
            if(spriteCharacters.Length != characters.Length)
            {
                return false;
            }

            dictionaryChar = new Dictionary<char, Sprite>();
            for (int index = 0; index < characters.Length; index++)
            {
                dictionaryChar.Add(characters[index], spriteCharacters[index]);
            }

            return true;
        }

        private void InstantiateImageCharacters(int length)
        {
            int childCount = transform.childCount;
            transform.ClearImmeadiate();

            bitmapCharacters = new List<BitmapCharacter>();
            for (int index = 0; index < length; index++)
            {
                bitmapCharacters.Add(new BitmapCharacter(string.Format("Character{0:00}", index), transform));
            }
        }

        #endregion Private Function

        #region Event



        #endregion Event

        #region Editor

        [ContextMenu("Test Text")]
        private void TestText()
        {
            layoutGroup = gameObject.AddComponentIfNotExist<GridLayoutGroup>();

            layoutGroup.cellSize = characterSize;
            layoutGroup.spacing = spacing;
            layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layoutGroup.constraintCount = maxCharacterInLine;
            layoutGroup.childAlignment = characterAlignment;

            InstantiateImageCharacters(length);

            isInitialized = MapCharacterAndSprite();

            Text = text;
        }

        #endregion Editor
    }
}