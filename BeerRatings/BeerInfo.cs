namespace BeerRatings
{
    public class BeerInfo
    {
        private readonly string _name;
        private readonly double? _rating;
        private readonly RatingSource _ratingSource;

        public BeerInfo(string name, double? rating, RatingSource ratingSource)
        {
            _name = name;
            _rating = rating;
            _ratingSource = ratingSource;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Rating
        {
            get { return _rating.HasValue ? (_rating.Value).ToString("P1") : "?"; }
        }

        public RatingSource RatingSource
        {
            get { return _ratingSource; }
        }

        public override string ToString()
        {
            return string.Format("Beer: {0}\t\tSource: {1}\t\tRating: {2}", Name, RatingSource, Rating);
        }
    }
}
