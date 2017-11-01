using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：角色别名
    public partial class T_ROLE_ALIAS:ModelBase
    {

                    
         #region  属性描述：ID
         
		 /// <summary>
         /// ID
         /// </summary>
         private int  _ID;
         
		 /// <summary>
         /// ID
         /// </summary>
         [Id(true)]
		 public int ID 
         {
           get
            {
                 return  _ID ;
            }
            set
            {
                Fields.Add("ID");   
                 _ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：团队ID
         
		 /// <summary>
         /// 团队ID
         /// </summary>
         private int  _RA_TEAM_ID;
         
		 /// <summary>
         /// 团队ID
         /// </summary>
         [Column]
		 public int RA_TEAM_ID 
         {
           get
            {
                 return  _RA_TEAM_ID ;
            }
            set
            {
                Fields.Add("RA_TEAM_ID");   
                 _RA_TEAM_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：角色CODE
         
		 /// <summary>
         /// 角色CODE
         /// </summary>
         private string  _RA_ROLE_CODE;
         
		 /// <summary>
         /// 角色CODE
         /// </summary>
         [Column]
		 public string RA_ROLE_CODE 
         {
           get
            {
                 return  _RA_ROLE_CODE ;
            }
            set
            {
                Fields.Add("RA_ROLE_CODE");   
                 _RA_ROLE_CODE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：角色别名
         
		 /// <summary>
         /// 角色别名
         /// </summary>
         private string  _RA_ROLE_ALIAS_NAME;
         
		 /// <summary>
         /// 角色别名
         /// </summary>
         [Column]
		 public string RA_ROLE_ALIAS_NAME 
         {
           get
            {
                 return  _RA_ROLE_ALIAS_NAME ;
            }
            set
            {
                Fields.Add("RA_ROLE_ALIAS_NAME");   
                 _RA_ROLE_ALIAS_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：创建人姓名
         
		 /// <summary>
         /// 创建人姓名
         /// </summary>
         private string  _CREATE_USER_MAIL;
         
		 /// <summary>
         /// 创建人姓名
         /// </summary>
         [Column]
		 public string CREATE_USER_MAIL 
         {
           get
            {
                 return  _CREATE_USER_MAIL ;
            }
            set
            {
                Fields.Add("CREATE_USER_MAIL");   
                 _CREATE_USER_MAIL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：创建时间
         
		 /// <summary>
         /// 创建时间
         /// </summary>
         private DateTime  _CREATE_TIME;
         
		 /// <summary>
         /// 创建时间
         /// </summary>
         [Column]
		 public DateTime CREATE_TIME 
         {
           get
            {
                 return  _CREATE_TIME ;
            }
            set
            {
                Fields.Add("CREATE_TIME");   
                 _CREATE_TIME  = value;
            }
         }     
        
         #endregion          
       
    }
}



	