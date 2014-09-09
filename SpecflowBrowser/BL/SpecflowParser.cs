namespace BL
{
    using System;
    using System.Linq;
    using DTO;
    using System.Globalization;
    using Bytel.Cora.Socle.Exception;
    using System.IO;

    public class SpecflowParser
    {
        private const string StrFeature = "Fonctionnalité:";
        private const string StrBackground = "Contexte:";
        private const string StrScenario = "Scénario:";
        //private const string StrScenarioOutline = "Plan du scénario";
        private const string StrGiven = "Etant donné";
        private const string StrWhen = "Quand";
        private const string StrThen = "Alors";
        private const string StrAnd = "Et";
        private const string StrBut = "Mais";
        //private const string StrExamples = "Exemples";

        public static Fonctionnalite Parse(string filename)
        {
            Fonctionnalite feature = new Fonctionnalite { Id = Guid.NewGuid(), Description = string.Empty, TestResult = TestResult.NoTest.ToString() };
            using (StreamReader sr = new StreamReader(filename))
            {
                try
                {
                    Scenario currentScenario = null;

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // skip comments
                        if (line.Contains("#"))
                        {
                            line = line.Split('#')[0];
                        }

                        // remove spaces on begin and end
                        line = line.Trim();

                        // skip if empty
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        if (line.StartsWith(StrFeature))
                        {
                            // fonctionnalité
                            feature.Nom = line.Replace(StrFeature, "").Trim();
                        }
                        else if (line.StartsWith(StrBackground))
                        {
                            // contexte
                            currentScenario = new Scenario() { Id = Guid.NewGuid(), Description = string.Empty, Nom = line.Replace(StrBackground, "").Trim() };
                            feature.ScenarioContext.Add(currentScenario);
                        }
                        else if (line.StartsWith(StrScenario))
                        {
                            // scénario
                            currentScenario = new Scenario() { Id = Guid.NewGuid(), Description = string.Empty, Nom = line.Replace(StrScenario, "").Trim() };
                            feature.Scenarios.Add(currentScenario);
                        }
                        else if (line.StartsWith(StrGiven))
                        {
                            // Given
                            currentScenario.Etapes.Add(new Etape
                            {
                                Id = Guid.NewGuid(),
                                Keyword = "Given",
                                NativeKeyword = StrGiven,
                                Nom = line.Replace(StrGiven, "").Trim(),
                                EtapeIndex = currentScenario.Etapes.Count
                            });
                        }
                        else if (line.StartsWith(StrWhen))
                        {
                            // When
                            currentScenario.Etapes.Add(new Etape
                            {
                                Id = Guid.NewGuid(),
                                Keyword = "When",
                                NativeKeyword = StrWhen,
                                Nom = line.Replace(StrWhen, "").Trim(),
                                EtapeIndex = currentScenario.Etapes.Count
                            });
                        }
                        else if (line.StartsWith(StrThen))
                        {
                            // Then
                            currentScenario.Etapes.Add(new Etape
                            {
                                Id = Guid.NewGuid(),
                                Keyword = "Then",
                                NativeKeyword = StrThen,
                                Nom = line.Replace(StrThen, "").Trim(),
                                EtapeIndex = currentScenario.Etapes.Count
                            });
                        }
                        else if (line.StartsWith(StrAnd))
                        {
                            // And
                            currentScenario.Etapes.Add(new Etape
                            {
                                Id = Guid.NewGuid(),
                                Keyword = "And",
                                NativeKeyword = StrAnd,
                                Nom = line.Replace(StrAnd, "").Trim(),
                                EtapeIndex = currentScenario.Etapes.Count
                            });
                        }
                        else if (line.StartsWith(StrBut))
                        {
                            // But
                            currentScenario.Etapes.Add(new Etape
                            {
                                Id = Guid.NewGuid(),
                                Keyword = "But",
                                NativeKeyword = StrBut,
                                Nom = line.Replace(StrBut, "").Trim(),
                                EtapeIndex = currentScenario.Etapes.Count
                            });
                        }
                        else if (line.StartsWith("|"))
                        {
                            // Tableau de valeurs
                            Etape etape = currentScenario.Etapes.Last();
                            string[] splitted = line.Split('|');
                            if (etape.Table == null)
                            {
                                etape.Table = new Table { Id = Guid.NewGuid() };

                                TableRow header = new TableRow { Id = Guid.NewGuid(), RowIndex = 0 };
                                for (int i = 1; i < splitted.Length - 1; i++)
                                {
                                    header.TableColonnes.Add(new TableColonne { Id = Guid.NewGuid(), ColIndex = header.TableColonnes.Count, Valeur = splitted[i].Trim() });
                                }
                                etape.Table.TableHeader.Add(header);
                            }
                            else
                            {
                                TableRow row = new TableRow { Id = Guid.NewGuid(), RowIndex = etape.Table.TableRows.Count };
                                for (int i = 1; i < splitted.Length - 1; i++)
                                {
                                    row.TableColonnes.Add(new TableColonne { Id = Guid.NewGuid(), ColIndex = row.TableColonnes.Count, Valeur = splitted[i].Trim() });
                                }
                                etape.Table.TableRows.Add(row);
                            }
                        }
                        else
                        {
                            // commentaire de fonctionnalité ou de scénario
                            if (currentScenario == null)
                            {
                                if (!string.IsNullOrEmpty(feature.Description))
                                {
                                    feature.Description += "\n";
                                }
                                feature.Description += line;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(currentScenario.Description))
                                {
                                    currentScenario.Description += "\n";
                                }
                                currentScenario.Description += line;
                            }
                        }
                    }
                }
                catch (IOException e)
                {
                    throw new ExceptionTechnique("Erreur lors de la lecture du fichier '" + filename + "' : " + e.Message, e);
                }

                sr.Close();
            }

            return feature;
        }
    }
}