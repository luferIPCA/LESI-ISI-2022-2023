/**
 * 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServiceExample.Model
{
    public class Aluno
    {
        string nome;
        int idade;

        public string Nome { get => nome; set => nome = value; }
        public int Idade { get => idade; set => idade = value; }


    }
}
