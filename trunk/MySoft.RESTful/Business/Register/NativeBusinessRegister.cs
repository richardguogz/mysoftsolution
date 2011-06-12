using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MySoft.RESTful.Business.Pool;
using MySoft.IoC;
using MySoft.Logger;

namespace MySoft.RESTful.Business.Register
{
    /// <summary>
    /// 本地业务注册器,读取本地配置文件,加载程序集,反射获取需要绑定的业务接口对象和业务方法
    /// </summary>
    public class NativeBusinessRegister : IBusinessRegister
    {
        private IBusinessPool pool;

        public void Register(IBusinessPool businessPool)
        {
            pool = businessPool;
            //读取配置文件
            try
            {
                BusinessKindModel kindModel = null;
                BusinessModel model = null;
                BusinessMetadata metadata = null;
                object instance = null;
                var container = CastleFactory.Create().ServiceContainer;
                foreach (Type serviceType in container.GetInterfaces<PublishKind>())
                {
                    //获取业务对象
                    instance = container[serviceType];

                    //获取类特性
                    var kind = CoreHelper.GetTypeAttribute<PublishKind>(serviceType);
                    if (kind != null)
                    {
                        kindModel = new BusinessKindModel();
                        kindModel.Name = kind.Name;
                        kindModel.Description = kind.Description;
                        pool.AddKindModel(kind.Name, kindModel);
                    }
                    else
                    {
                        continue;
                    }

                    //获取方法特性
                    MethodInfo[] methods = CoreHelper.GetMethodsFromType(serviceType);
                    foreach (MethodInfo info in methods)
                    {
                        var method = CoreHelper.GetMemberAttribute<PublishMethod>(info);
                        if (method != null)
                        {
                            CreateBusinessModel(method, ref kindModel, ref model);
                            CreateBusinessMetadata(info, ref model, ref metadata);
                            metadata.Type = method.Method;
                            metadata.Instance = instance;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SimpleLog.Instance.WriteLog(ex);
            }
        }

        /// <summary>
        /// 创建业务元数据
        /// </summary>
        /// <param name="info"></param>
        /// <param name="model"></param>
        /// <param name="metadata"></param>
        private void CreateBusinessMetadata(MethodInfo info, ref BusinessModel model, ref BusinessMetadata metadata)
        {
            IList<BusinessMetadata> metadatas = model.Metadatas;
            if (metadatas == null)
            {
                metadatas = new List<BusinessMetadata>();
            }
            ParameterInfo[] parameters = info.GetParameters();
            int parametersCount = parameters.Length;
            metadata = metadatas.Where(e => e.ParametersCount == parametersCount).SingleOrDefault();
            if (metadata == null)
            {
                metadata = new BusinessMetadata();
                metadata.Method = info;
                metadata.Parameters = parameters;
                metadata.ParametersCount = parametersCount;
                metadatas.Add(metadata);
            }
            model.Metadatas = metadatas;
        }

        /// <summary>
        /// 创建业务模型
        /// </summary>
        /// <param name="publishMethod"></param>
        /// <param name="kindModel"></param>
        /// <param name="model"></param>
        private void CreateBusinessModel(PublishMethod publishMethod, ref BusinessKindModel kindModel, ref BusinessModel model)
        {
            string name = publishMethod.Name;
            IDictionary<string, BusinessModel> models = kindModel.Models;
            if (models == null)
            {
                models = new Dictionary<string, BusinessModel>();
            }
            model = models.Where(e => e.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Select(v => v.Value).SingleOrDefault();
            if (model == null)
            {
                model = new BusinessModel();
                model.Name = name;
                model.Description = publishMethod.Description;
                models.Add(name, model);
            }
            kindModel.Models = models;
        }
    }
}
