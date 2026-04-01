    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BKMusic : MonoBehaviour
    {
        private static BKMusic instance;
        public static BKMusic Instance => instance;
        private AudioSource bkSource;
        private void Awake()
        {
            instance = this;
            bkSource = GetComponent<AudioSource>();
            //通过数据来设置音乐的大小和开关
            MusicData data = GameDataMgr.Instance.musicData;
            SetIsOpen(data.musicOpen);
            ChangeValue(data.musicValue);
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        //开关背景音乐
        public void SetIsOpen(bool isOpen)
        {
            bkSource.mute = !isOpen;
        }
        //调整背景音乐
        public void ChangeValue(float v)
        {
            bkSource.volume = v;
        }
    }
