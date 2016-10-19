namespace TrainRoute.Business
{
    public class Vertice
    {
        public string Id { get; set; }

        public Vertice(string id)
        {
            this.Id = id;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 3) + this.Id.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Vertice p = (Vertice)obj;
            return this.Id.Equals(p.Id);
        }

        public override string ToString()
        {
            return this.Id;
        }
    }
}
