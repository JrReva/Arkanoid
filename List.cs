using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Arkanoid
{
    public class List<T> : IEnumerable
    {
        private T[] _data = new T[0];

        /// <summary>
        /// Ajouter un objet à la liste
        /// </summary>
        /// <param name="objet">L'objet à ajouter</param>
        public void Ajouter(T objet)
        {
            //On crée le nouveau tableau
            T[] temp = new T[_data.Length + 1];

            //On copie les données de l'autre et on set le nouveau au dernier index
            _data.CopyTo(temp, 0);
            temp[_data.Length] = objet;

            //Puis on change la référence
            _data = temp;
        }

        /// <summary>
        /// Enlever un élément
        /// </summary>
        /// <param name="objet">L'élément à enlever</param>
        public void Enlever(T objet)
        {
            //S'il ne contient pas l'objet, on sort
            if(!_data.Contains(objet))
                return;

            //On crée un nouveau tableau
            T[] temp = new T[_data.Length - 1];
            int index = 0;

            //On parcourt le tableau, si l'objet n'est pas celui recherché, on le met dans le nouveau tableau
            for (int i = 0; i < _data.Length; i++)
                 if (!_data[i].Equals(objet))
                     temp[index++] = _data[i];

            //Puis on change les références
            _data = temp;
        }

        public T Pop()
        {
            if (Count == 0)
                return default(T);

            T temp = _data[Count - 1];
            Enlever(temp);
            return temp;
        }

        /// <summary>
        /// Retourne le nombre d'éléments
        /// </summary>
        public int Count
        {
            get{ return _data.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
