using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Reflection;

namespace Mintcode.TuoTuo.v2.AutoMapper
{
    public class Configuration
    {
        public static void Configure()
        {
            var assembly = Assembly.Load("Mintcode.TuoTuo.v2.AutoMapper");
            var profiles = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Profile))).ToList();
            Mapper.Initialize(cfg =>
            {
                profiles.ForEach(profile =>
                {
                    cfg.AddProfile(profile);
                });
            });
        }
    }
}
