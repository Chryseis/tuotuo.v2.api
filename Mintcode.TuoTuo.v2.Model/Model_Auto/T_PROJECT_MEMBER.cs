using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：
    public partial class T_PROJECT_MEMBER:ModelBase
    {

                    
         #region  属性描述：
         
		 /// <summary>
         /// 
         /// </summary>
         private int  _ID;
         
		 /// <summary>
         /// 
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
                  
         #region  属性描述：项目ID
         
		 /// <summary>
         /// 项目ID
         /// </summary>
         private int  _P_PROJECT_ID;
         
		 /// <summary>
         /// 项目ID
         /// </summary>
         [Column]
		 public int P_PROJECT_ID 
         {
           get
            {
                 return  _P_PROJECT_ID ;
            }
            set
            {
                Fields.Add("P_PROJECT_ID");   
                 _P_PROJECT_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：项目成员ID
         
		 /// <summary>
         /// 项目成员ID
         /// </summary>
         private int  _U_USER_ID;
         
		 /// <summary>
         /// 项目成员ID
         /// </summary>
         [Column]
		 public int U_USER_ID 
         {
           get
            {
                 return  _U_USER_ID ;
            }
            set
            {
                Fields.Add("U_USER_ID");   
                 _U_USER_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：成员角色
         
		 /// <summary>
         /// 成员角色
         /// </summary>
         private string  _R_ROLE_CODE;
         
		 /// <summary>
         /// 成员角色
         /// </summary>
         [Column]
		 public string R_ROLE_CODE 
         {
           get
            {
                 return  _R_ROLE_CODE ;
            }
            set
            {
                Fields.Add("R_ROLE_CODE");   
                 _R_ROLE_CODE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：标签
         
		 /// <summary>
         /// 标签
         /// </summary>
         private string  _P_TAGS;
         
		 /// <summary>
         /// 标签
         /// </summary>
         [Column]
		 public string P_TAGS 
         {
           get
            {
                 return  _P_TAGS ;
            }
            set
            {
                Fields.Add("P_TAGS");   
                 _P_TAGS  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：0 已邀请 1 已同意
         
		 /// <summary>
         /// 0 已邀请 1 已同意
         /// </summary>
         private int  _T_STATE;
         
		 /// <summary>
         /// 0 已邀请 1 已同意
         /// </summary>
         [Column]
		 public int T_STATE 
         {
           get
            {
                 return  _T_STATE ;
            }
            set
            {
                Fields.Add("T_STATE");   
                 _T_STATE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：项目成员名字
         
		 /// <summary>
         /// 项目成员名字
         /// </summary>
         private string  _U_USER_NAME;
         
		 /// <summary>
         /// 项目成员名字
         /// </summary>
         [Column]
		 public string U_USER_NAME 
         {
           get
            {
                 return  _U_USER_NAME ;
            }
            set
            {
                Fields.Add("U_USER_NAME");   
                 _U_USER_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：项目成员邮箱
         
		 /// <summary>
         /// 项目成员邮箱
         /// </summary>
         private string  _U_USER_EMAIL;
         
		 /// <summary>
         /// 项目成员邮箱
         /// </summary>
         [Column]
		 public string U_USER_EMAIL 
         {
           get
            {
                 return  _U_USER_EMAIL ;
            }
            set
            {
                Fields.Add("U_USER_EMAIL");   
                 _U_USER_EMAIL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：创建人
         
		 /// <summary>
         /// 创建人
         /// </summary>
         private string  _CREATE_USER_MAIL;
         
		 /// <summary>
         /// 创建人
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



	