using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：角色表
    public partial class T_ROLE:ModelBase
    {

                    
         #region  属性描述：角色ID
         
		 /// <summary>
         /// 角色ID
         /// </summary>
         private int  _ID;
         
		 /// <summary>
         /// 角色ID
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
                  
         #region  属性描述：角色CODE
         
		 /// <summary>
         /// 角色CODE
         /// </summary>
         private string  _R_CODE;
         
		 /// <summary>
         /// 角色CODE
         /// </summary>
         [Column]
		 public string R_CODE 
         {
           get
            {
                 return  _R_CODE ;
            }
            set
            {
                Fields.Add("R_CODE");   
                 _R_CODE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：角色名称
         
		 /// <summary>
         /// 角色名称
         /// </summary>
         private string  _R_NAME;
         
		 /// <summary>
         /// 角色名称
         /// </summary>
         [Column]
		 public string R_NAME 
         {
           get
            {
                 return  _R_NAME ;
            }
            set
            {
                Fields.Add("R_NAME");   
                 _R_NAME  = value;
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



	