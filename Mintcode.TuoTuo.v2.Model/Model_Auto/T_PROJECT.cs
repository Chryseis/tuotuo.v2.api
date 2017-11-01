using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：
    public partial class T_PROJECT:ModelBase
    {

                    
         #region  属性描述：主键
         
		 /// <summary>
         /// 主键
         /// </summary>
         private int  _ID;
         
		 /// <summary>
         /// 主键
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
                  
         #region  属性描述：项目名称
         
		 /// <summary>
         /// 项目名称
         /// </summary>
         private string  _P_PROJECT_NAME;
         
		 /// <summary>
         /// 项目名称
         /// </summary>
         [Column]
		 public string P_PROJECT_NAME 
         {
           get
            {
                 return  _P_PROJECT_NAME ;
            }
            set
            {
                Fields.Add("P_PROJECT_NAME");   
                 _P_PROJECT_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：项目简介
         
		 /// <summary>
         /// 项目简介
         /// </summary>
         private string  _P_PROJECT_SUMMARY;
         
		 /// <summary>
         /// 项目简介
         /// </summary>
         [Column]
		 public string P_PROJECT_SUMMARY 
         {
           get
            {
                 return  _P_PROJECT_SUMMARY ;
            }
            set
            {
                Fields.Add("P_PROJECT_SUMMARY");   
                 _P_PROJECT_SUMMARY  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：项目图片
         
		 /// <summary>
         /// 项目图片
         /// </summary>
         private string  _P_PROJECT_AVATAR;
         
		 /// <summary>
         /// 项目图片
         /// </summary>
         [Column]
		 public string P_PROJECT_AVATAR 
         {
           get
            {
                 return  _P_PROJECT_AVATAR ;
            }
            set
            {
                Fields.Add("P_PROJECT_AVATAR");   
                 _P_PROJECT_AVATAR  = value;
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



	