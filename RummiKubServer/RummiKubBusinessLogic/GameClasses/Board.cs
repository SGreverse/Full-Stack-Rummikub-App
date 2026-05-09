using RummiKubClasses.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.GamePieces
{
    //[DataContract]
    public class Board
    {
        //[DataMember]
        public List<TileSeries> OnBoardSeries { get; set; }
        //[DataMember]
        public int TilesOnBoard {  get; set; }
        //[DataMember]
        public int Series_On_Board_Amount {  get; set; }
        public Board()//constructor for the first board creation
        {
            OnBoardSeries = new List<TileSeries>();
            TilesOnBoard = 0;
            Series_On_Board_Amount = 0;
        }
        public Board(Board other) // constructor to save the start of turn board in case of changes 
        {
            this.OnBoardSeries =CopyOnBoardSeries(other.OnBoardSeries);

            this.TilesOnBoard = other.TilesOnBoard;

            this.Series_On_Board_Amount=other.Series_On_Board_Amount;
        }
        public List<TileSeries> CopyOnBoardSeries(List<TileSeries> series_on_board)
        {
            List<TileSeries> newlist= new List<TileSeries>();
            foreach(TileSeries series in series_on_board)
            {
                try
                {
                    newlist.Add(new ColorSeries(new List<Tile>(series.set),series.SeriesId));
                    continue;
                }
                catch { }
                try
                {
                    newlist.Add(new RisingSeries(new List<Tile>(series.set),series.SeriesId));
                    continue;
                }
                catch {}
            }
            return newlist;
        }
        public void AddSeries(TileSeries series)
        {
            this.Series_On_Board_Amount++;
            series.SeriesId = this.Series_On_Board_Amount;
            this.OnBoardSeries.Add(series);
            this.TilesOnBoard+=series.set.Count;

        }

        public void AddTile(int seriesId,Tile tile)
        {
            TileSeries series=GetSeriesById(seriesId);
            series.TryAddTile(tile);
            this.TilesOnBoard++;

        }
        public void RemoveTile(int seriesId,Tile t)
        {
            TileSeries series = GetSeriesById(seriesId);
            series.BorrowTile(t);
            if (series.set.Count == 0)
            {
                this.OnBoardSeries.Remove(series);
            }
            TilesOnBoard--;
        }
        public TileSeries GetSeriesById(int Id)
        {
            return this.OnBoardSeries.First(s=>s.SeriesId==Id);
        }
        public bool ValidateBoard()
        {
            for(int i = 0;i<this.Series_On_Board_Amount;i++)
            {
                try
                {
                    this.OnBoardSeries[i] = new ColorSeries(this.OnBoardSeries[i].set, this.OnBoardSeries[i].SeriesId);
                    continue;
                }
                catch { }
                try
                {
                    this.OnBoardSeries[i] = new RisingSeries(this.OnBoardSeries[i].set, this.OnBoardSeries[i].SeriesId);
                    continue;

                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        public bool HasChanged(int tilesonboard) // for checking in the end of the turn if the player put a tile on the board
        {
            return !(tilesonboard == this.TilesOnBoard); 
        }
        public void ClearBoard()
        {
            this.OnBoardSeries.Clear();
            this.TilesOnBoard = 0;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Board) || this==null)
                return false;
            if ((obj as Board).TilesOnBoard != this.TilesOnBoard)
                return false;
            foreach(TileSeries series in (obj as Board).OnBoardSeries)
            {
                if(!this.OnBoardSeries.Contains(series)) return false;
            }
            return true;
        }

    }
}
