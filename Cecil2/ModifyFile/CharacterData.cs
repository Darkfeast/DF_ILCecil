using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModifyFile
{
    [Serializable]
    public class CharacterData{

        public List<Character> characterData;
    }

    [Serializable]
    public class Character
    {
        public int ID;
        public string mainCard;
        public string subCard;
        public string isFlip;
        public string isAnim;
        public string errCard;
        public string errNew;
        public int topic;
        public int unit;
        public string mainAudioName;
        public string subAudioName;
        public string combo;
        public string comboType;
        public string singleAudioName;
        public string prompt;
        public string promptType;
    }
}
