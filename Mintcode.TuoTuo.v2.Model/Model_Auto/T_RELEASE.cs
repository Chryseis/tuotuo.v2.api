using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：
    public partial class T_RELEASE:ModelBase
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
                  
         #region  属性描述：团队ID
         
		 /// <summary>
         /// 团队ID
         /// </summary>
         private int  _T_TEAM_ID;
         
		 /// <summary>
         /// 团队ID
         /// </summary>
         [Column]
		 public int T_TEAM_ID 
         {
           get
            {
                 return  _T_TEAM_ID ;
            }
            set
            {
                Fields.Add("T_TEAM_ID");   
                 _T_TEAM_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：周期名称
         
		 /// <summary>
         /// 周期名称
         /// </summary>
         private string  _R_NAME;
         
		 /// <summary>
         /// 周期名称
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
                  
         #region  属性描述：周期简介
         
		 /// <summary>
         /// 周期简介
         /// </summary>
         private string  _R_SUMMARY;
         
		 /// <summary>
         /// 周期简介
         /// </summary>
         [Column]
		 public string R_SUMMARY 
         {
           get
            {
                 return  _R_SUMMARY ;
            }
            set
            {
                Fields.Add("R_SUMMARY");   
                 _R_SUMMARY  = value;
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



	