
namespace HowToDoIt.App_Start
{
    using HowToDoIt.Models.Classes_for_Db;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    public class LuceneSearchConfig
    {
        public static Directory directory;
        public static Analyzer analyzer;
        public static IndexWriter writer;

        public static void InitializeSearch()
        {
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LuceneIndexes";
            directory = FSDirectory.Open(directoryPath);
            analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        public static void DeleteDocument(int id)
        {
            writer.DeleteDocuments(new Lucene.Net.Index.Term("Id", Convert.ToString(id)));
            writer.Optimize();
            writer.Commit();
        }

        public static void CreateDocument(string id, string name)
        {
            Document doc = new Document();
            doc.Add(new Field("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", name, Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(doc);
            writer.Optimize();
            writer.Commit();
        }

        public static void CreateIndex(Instruction instruction)
        {
            CreateDocument(instruction.Id.ToString(), instruction.Name);
            if (instruction.Steps!=null)
            {
                foreach(var step in instruction.Steps.ToList())
                {
                    if (step.Blocks!=null)
                    {
                        CreateDocument(instruction.Id.ToString(), step.Name);
                        foreach (var block in step.Blocks)
                        {
                            if (block.Type=="Text")
                            {
                                CreateDocument(instruction.Id.ToString(), block.Name);
                            }
                        }
                    }
                }
            }
        }

        public static List<int> Search(string str)
        {
            var query = str;
            string indexDirectory = HttpContext.Current.Server.MapPath("~/App_Data/LuceneIndexes");
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexSearcher searcher = new IndexSearcher(FSDirectory.Open(indexDirectory));
            var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Name", analyzer);
            Query searchQuery = parser.Parse(query);
            TopDocs hits = searcher.Search(searchQuery, 200);
            int results = hits.TotalHits;
            List<int> list = new List<int>();
            for (int i = 0; i < results; i++)
            {
                Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
                Instruction instr = new Instruction();
                list.Add(Int32.Parse(doc.Get("Id")));
            }
            list = list.Distinct().ToList();
            return list;
        }

        /*public static void FinalizeSearch()
        {
            writer.Dispose();
            directory.Dispose();
        }*/

        
    }
}