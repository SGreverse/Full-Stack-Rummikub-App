using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.Tiles;
namespace RummiKubClasses.GamePieces
{
    //[DataContract]
    public class TileBank
    {
        private Stack<Tile> Bank;
        public TileBank()
        {
            Bank=new Stack<Tile>();
            Fill();
        }
        public void Fill()
        {
            for (int i = 0; i < 2; i++) {
                foreach (RummiColor rummiColor in Enum.GetValues(typeof(RummiColor)))
                {
                    for(int j = 1; j < 14; j++)
                    {
                        this.Bank.Push(new Tile(j, rummiColor));
                    }
                }
            }
            this.Bank.Push(new Joker(RummiColor.Black));
            this.Bank.Push(new Joker(RummiColor.Red));
            Shuffle();
        }
        private void Shuffle()
        {
           List<Tile> list=this.Bank.ToList();
           Random random = new Random();
           for (int i = 0; i < list.Count; i++)
            {
                int num = random.Next(list.Count);
                Tile temp = list[i];
                list[i] = list[num];
                list[num] = temp;
            }
            this.Bank.Clear();
            foreach(Tile tile in list)
            {
                this.Bank.Push(tile);
            }
        }
        public Tile Draw()
        {

            return this.Bank.Pop();
        }
        public bool IsEmpty()
        {
            return this.Bank.Count == 0;
        }
    }
}
