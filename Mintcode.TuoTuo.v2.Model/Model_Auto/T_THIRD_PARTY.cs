using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：第三方登陆
    public partial class T_THIRD_PARTY:ModelBase
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
                  
         #region  属性描述：用户ID
         
		 /// <summary>
         /// 用户ID
         /// </summary>
         private int  _U_USER_ID;
         
		 /// <summary>
         /// 用户ID
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
                  
         #region  属性描述：第三方ID
         
		 /// <summary>
         /// 第三方ID
         /// </summary>
         private string  _T_THIRD_PARTY_ID;
         
		 /// <summary>
         /// 第三方ID
         /// </summary>
         [Column]
		 public string T_THIRD_PARTY_ID 
         {
           get
            {
                 return  _T_THIRD_PARTY_ID ;
            }
            set
            {
                Fields.Add("T_THIRD_PARTY_ID");   
                 _T_THIRD_PARTY_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：来源
         
		 /// <summary>
         /// 来源
         /// </summary>
         private string  _T_FROM;
         
		 /// <summary>
         /// 来源
         /// </summary>
         [Column]
		 public string T_FROM 
         {
           get
            {
                 return  _T_FROM ;
            }
            set
            {
                Fields.Add("T_FROM");   
                 _T_FROM  = value;
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



	