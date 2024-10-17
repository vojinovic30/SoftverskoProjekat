namespace startApp.Classes
{
    public class Mobilni
    { 
        public int id {get; set;}
        public string brand {get; set;}
        public string model {get;set;}

        public string tip { get; set; }

        public override bool Equals(object obj)
        {
            if(object.ReferenceEquals(this,obj))
                return true;

            if(obj ==null || obj.GetType() !=typeof(Mobilni))
                return false;

            Mobilni pom =(Mobilni)obj;

            if(pom!=null && pom.brand==this.brand && pom.model==this.model && pom.tip==this.tip)
                return true;

            return false;
        }

        
    }
}