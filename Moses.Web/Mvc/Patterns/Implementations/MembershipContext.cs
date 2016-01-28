using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Moses.Web.Mvc.Patterns
{

    public class MembershipContextFactory
    {
        public static MembershipContext Initialize(System.Web.HttpContextBase httpContext)
        {
            MembershipContext membershipContext = new MembershipContext(httpContext);
            membershipContext.Initialize();
            return membershipContext as MembershipContext;
        }

        public static MembershipContext<T> Initialize<T>(System.Web.HttpContextBase httpContext)
            where T : class ,  new() 
            
        {
            MembershipContext<T> membershipContext = new MembershipContext<T>(httpContext);
            membershipContext.Initialize();
            return membershipContext as MembershipContext<T>;
        }
    }

    public class MembershipContext : MembershipContext<object>
    {
        public MembershipContext(System.Web.HttpContextBase httpContextWrapper)
            : base(httpContextWrapper)
        {

        }
    }

    public class MembershipContext<T> 
        where T : class , new() 
        
    {
        private System.Web.HttpContextBase _httpContextWrapper;

        public MembershipContext(System.Web.HttpContextBase httpContextWrapper)
        {
            // TODO: Complete member initialization
            this._httpContextWrapper = httpContextWrapper;
        }

        public HttpContextBase CurrentContext { get { return _httpContextWrapper; } }
        public HttpSessionStateBase Session { get
            {
                return _httpContextWrapper.Session;
            }
        }

        public IUser User
        {
            get;
            set;
        }

        public bool HasPermission(params string[] tResult)
        {
            return true;
        }

        public IContract Contract
        {
            get;
            set;
        }

        public T Info { get; set; }

        
        public virtual void OnInitializeUser()
        {
            if ( Configuration.HasUserInitializer)
                User = Configuration.ExecuteUserInitializer(this.CurrentContext, this.Session);
        }

        public virtual void OnInitializeContract()
        {
            if (Configuration.HasContractInitializer)
                Contract = Configuration.ExecuteContractInitializer(this.CurrentContext, this.Session);
        }

        public virtual void OnInitializeInfo()
        {
            if (Configuration.HasInfoInitializer)
                Info = Configuration.ExecuteInfoInitializer(this.CurrentContext, this.Session) as T;
        }

        public void Initialize()
        {
            this.OnInitializeUser();
            this.OnInitializeContract();
            this.OnInitializeInfo();
        }


        public string AvailableContractsList { get; set; }
    }
}
