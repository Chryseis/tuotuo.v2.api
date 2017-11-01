using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：团队表
    public partial class T_TEAM:ModelBase
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
                  
         #region  属性描述：团队code
         
		 /// <summary>
         /// 团队code
         /// </summary>
         private string  _T_TEAM_CODE;
         
		 /// <summary>
         /// 团队code
         /// </summary>
         [Column]
		 public string T_TEAM_CODE 
         {
           get
            {
                 return  _T_TEAM_CODE ;
            }
            set
            {
                Fields.Add("T_TEAM_CODE");   
                 _T_TEAM_CODE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：团队名称
         
		 /// <summary>
         /// 团队名称
         /// </summary>
         private string  _T_TEAM_NAME;
         
		 /// <summary>
         /// 团队名称
         /// </summary>
         [Column]
		 public string T_TEAM_NAME 
         {
           get
            {
                 return  _T_TEAM_NAME ;
            }
            set
            {
                Fields.Add("T_TEAM_NAME");   
                 _T_TEAM_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：团队简介
         
		 /// <summary>
         /// 团队简介
         /// </summary>
         private string  _T_TEAM_SUMMARY;
         
		 /// <summary>
         /// 团队简介
         /// </summary>
         [Column]
		 public string T_TEAM_SUMMARY 
         {
           get
            {
                 return  _T_TEAM_SUMMARY ;
            }
            set
            {
                Fields.Add("T_TEAM_SUMMARY");   
                 _T_TEAM_SUMMARY  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：团队图片
         
		 /// <summary>
         /// 团队图片
         /// </summary>
         private string  _T_TEAM_AVATAR;
         
		 /// <summary>
         /// 团队图片
         /// </summary>
         [Column]
		 public string T_TEAM_AVATAR 
         {
           get
            {
                 return  _T_TEAM_AVATAR ;
            }
            set
            {
                Fields.Add("T_TEAM_AVATAR");   
                 _T_TEAM_AVATAR  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：团队状态(0=未启用,1=启用)
         
		 /// <summary>
         /// 团队状态(0=未启用,1=启用)
         /// </summary>
         private int  _T_STATUS;
         
		 /// <summary>
         /// 团队状态(0=未启用,1=启用)
         /// </summary>
         [Column]
		 public int T_STATUS 
         {
           get
            {
                 return  _T_STATUS ;
            }
            set
            {
                Fields.Add("T_STATUS");   
                 _T_STATUS  = value;
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



	