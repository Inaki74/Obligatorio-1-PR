using System;

namespace Domain.BusinessObjects
{
    public class Review
    {
        //CRF5. Calificar un juego. Se deberá poder calificar un juego, agregando un breve comentario
        //acerca de la opinión del mismo y una nota que debe estar comprendida en un rango a definir
        //por el usuario.
        public int ID {get; set;}
        public User ReviewPublisher {get; set;}
        public Game Game {get; set;}
        public int Score {get; set;}
        public string Description {get; set;}

        public Review()
        {

        }
    }
}
