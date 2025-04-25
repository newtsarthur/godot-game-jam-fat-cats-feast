// using Godot;
// using System;

// public partial class Control : Node2D
// {
//       private Label meuLabel; // Referência ao Label
//     private int pontuacao = 0; // Exemplo de variável

//     public override void _Ready()
//     {
//         // Pega o nó Label (ajuste o caminho conforme sua cena)
//         meuLabel = GetNode<Label>("Label");
        
//         // Atualiza o texto inicial
//         AtualizarTexto();
//     }

//     private void AtualizarTexto()
//     {
//         // Formata a variável no texto do Label
//         meuLabel.Text = $"Pontuação: {pontuacao}";
//     }

//     // Exemplo: se a pontuação mudar em algum momento
//     public void AumentarPontuacao()
//     {
//         pontuacao += 10;
//         AtualizarTexto(); // Atualiza o Label
//     }
// }
