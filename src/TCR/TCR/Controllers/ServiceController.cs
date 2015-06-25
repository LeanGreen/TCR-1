﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TCR.Models;

namespace TCR.Controllers
{
    public class HeatmapDataContainer
    {
        public List<string> Reperoires { get; set; }

        public List<int[]> Data { get; set; }
    };

    [Authorize]
    public class ServiceController : ApiController
    {
        private TCRContext db = new TCRContext();

        [Route("api/Service/GetLengthDiv/{id}")]
        public IEnumerable<int[]> GetLengthDiv(int id)
        {
            var personalReceptors = db.PersonalReceptors.Where(rec => rec.PersonId == id).Include(p => p.Receptor);
            int[] len = new int[100];
            foreach (var pr in personalReceptors)
            {
                len[pr.Receptor.NucleoSequence.Length] += pr.ReadCount;
            }

            var result = new List<int[]>();
            for (int i = 1; i < 100; ++i)
                if (len[i] > 0)
                    result.Add(new int[2] { i, len[i] });
            return result;
        }

        [Route("api/Service/GetVsegDiv/{id}")]
        public IEnumerable<KeyValuePair<string, int>> GetVsegDiv(int id)
        {
            var receptors = db.PersonalReceptors
                .Where(rec => rec.PersonId == id)
                .Include(p => p.Receptor)
                .Select(p => p.Receptor)
                .Include(p => p.VSegments);
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (var rec in receptors)
            {
                foreach (var vseg in rec.VSegments)
                    if (dic.ContainsKey(vseg.Alleles))
                        dic[vseg.Alleles] += 1;
                    else
                        dic.Add(vseg.Alleles, 1);
            }
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int> >();
            foreach (KeyValuePair<string, int> kv in dic)
                result.Add(kv);

            return result;
        }

        [Route("api/Service/GetClones")]
        public HeatmapDataContainer GetClones()
        {
            var res = new HeatmapDataContainer()
            {
                Reperoires = new List<string>() { "Rep1", "Rep2", "Rep3", "Rep4", "Rep5", "Rep6" },
                Data = new List<int[]>() { new int[3] { 0, 0, 10 }, new int[3] { 0, 1, 20 }, new int[3] { 2, 3, 50 } }
            };

            return res;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}