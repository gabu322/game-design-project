using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CicloDiaNoite : MonoBehaviour
{
    [SerializeField] private Transform luzDirecional; // Transform da luz direcional
    [SerializeField] [Tooltip("Duração do dia em segundos")] private int duracaoDoDia; // Duração do dia em segundos
    [SerializeField] private TextMeshProUGUI horarioText; // Texto para exibir o horário atual

    private float segundos;
    private float multiplicador;

    // Start is called before the first frame update
    void Start()
    {
        multiplicador = 86400 / duracaoDoDia;
        
    }

    // Update is called once per frame
    void Update()
    {
        segundos += Time.deltaTime * multiplicador;

        if (segundos >= 86400)
        {
            segundos = 0;
        }

        ProcessarCeu();
        CalcularHorario();
        
    }

    private void ProcessarCeu()
    {
        float rotacaoX = Mathf.Lerp(-90, 270, segundos / 86400);
        luzDirecional.rotation = Quaternion.Euler(rotacaoX, 0, 0);
    }

    private void CalcularHorario()
    {
        horarioText.text = TimeSpan.FromSeconds(segundos).ToString(@"hh\:mm");
    }
}