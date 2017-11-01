using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.TuoTuo.v2.Model;
using Mintcode.Zeus.Public.Data;

namespace Mintcode.TuoTuo.v2.BLL
{
    public partial class T_USER_BLL
    {
        public bool AddUserAndThirdParty(T_USER user,string thirdPartyId,string from)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    DbContext.Insert(user);

                    var thirdPartyEntity = new T_THIRD_PARTY();
                    thirdPartyEntity.U_USER_ID = user.ID;
                    thirdPartyEntity.T_THIRD_PARTY_ID = thirdPartyId;
                    thirdPartyEntity.T_FROM = from;
                    thirdPartyEntity.CREATE_TIME = DateTime.Now;
                    DbContext.Insert(thirdPartyEntity);
                }
                catch (Exception e)
                {
                    isSucess = false;
                    throw e;
                }
                finally
                {
                    if (isSucess)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                return isSucess;
            }
        }

        public bool UpdateUserAndThirdParty(T_USER user, string thirdPartyId, string from)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    DbContext.Update(user);

                    var thirdPartyEntity = new T_THIRD_PARTY();
                    thirdPartyEntity.U_USER_ID = user.ID;
                    thirdPartyEntity.T_THIRD_PARTY_ID = thirdPartyId;
                    thirdPartyEntity.T_FROM = from;
                    thirdPartyEntity.CREATE_TIME = DateTime.Now;
                    DbContext.Insert(thirdPartyEntity);
                }
                catch (Exception e)
                {
                    isSucess = false;
                    throw e;
                }
                finally
                {
                    if (isSucess)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                return isSucess;
            }
        }
    }
}
