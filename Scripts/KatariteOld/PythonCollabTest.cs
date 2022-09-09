using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;

public class PythonCollabTest : MonoBehaviour
{
    //python���s�t�@�C���A�h���X�w��
    private string pyExePath = @"python.exe";

    //Python�\�[�X�R�[�h�t�@�C���A�h���X�w��
    private string pyCodePath = @"C:\Users\mictle\Documents\PythonScripts\katariteEx\unityTest.py";

    // Start is called before the first frame update
    void Start()
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo()
        {
            FileName = pyExePath,
            UseShellExecute = false, //�V�F�����J�����ǂ���
            CreateNoWindow = true, //�V�����E�C���h�E���J�����ǂ���
            RedirectStandardOutput = true, //�e�L�X�g�o�͂�StandardOutput�X�g���[���ɏ������ނ��ǂ���
            Arguments = pyCodePath + " " + "Hello, Python.", //���s�X�N���v�g�̈���
        };

        //�O���v���Z�X�J�n
        Process process = Process.Start(processStartInfo);

        //�X�g���[������o�͂𓾂�
        StreamReader streamReader = process.StandardOutput;
        string str = streamReader.ReadLine();

        //�O���v���Z�X�I��
        process.WaitForExit();
        process.Close();

        print(str);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
