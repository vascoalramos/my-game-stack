import csv
import sys

csv.field_size_limit(sys.maxsize)

def isfloat(x):
    try:
        a = float(x)
    except ValueError:
        return False
    else:
        return True
    return True

def isint(x):
    try:
        a = float(x)
        b = int(a)
    except ValueError:
        return False
    else:
        return a == b

def csv_to_sql(baseFile, sqlFile, tableName):

    text_file = open(sqlFile, "w")
    openFile = open(baseFile, 'r')
    csvFile = csv.reader(openFile)
    firstLine=True

    for row in csvFile:

        if(not firstLine):
            #begin the line
            line = "INSERT INTO " + tableName + " VALUES ( "

            #counter
            i=0

            for value in row:
                value = value.replace("\"","'")

                if i == 0:
                    i += 1
                    continue

                #check if is the last value
                if(i == len(row)-1 ):
                    if(isint(value) or isfloat(value)):
                        line += value + " )"
                    else:
                        if not value: 
                            line+= " NULL ) "
                        elif value[0] == "'":
                            line+=value+ " ) "
                        else:
                            line+="'"+value+"'" + " )"

                else:
                    if(isint(value) or isfloat(value)):
                        line += value + ", "
                    else:
                        if not value: 
                            line+= " NULL, "
                        elif value[0] == "'":
                            line+=value+ ", "
                        else:
                            line+="'"+value+"'" + ", "

                i +=1

            line += "\nGO"
            text_file.write(line+"\n")

        firstLine=False

    openFile.close()
    text_file.close()

def csv_to_sql2(baseFile, sqlFile, tableName):

    text_file = open(sqlFile, "w")
    openFile = open(baseFile, 'r')
    csvFile = csv.reader(openFile)
    firstLine=True

    for row in csvFile:

        if(not firstLine):
            #begin the line
            line = "INSERT INTO " + tableName + " VALUES ( "

            #counter
            i=0

            for value in row:
                value = value.replace("\"","'")

                #check if is the last value
                if(i == len(row)-1 ):
                    if(isint(value) or isfloat(value)):
                        line += value + " )"
                    else:
                        if not value: 
                            line+= " NULL ) "
                        elif value[0] == "'":
                            line+=value+ " ) "
                        else:
                            line+="'"+value+"'" + " )"

                else:
                    if(isint(value) or isfloat(value)):
                        line += value + ", "
                    else:
                        if not value: 
                            line+= " NULL, "
                        elif value[0] == "'":
                            line+=value+ ", "
                        else:
                            line+="'"+value+"'" + ", "

                i +=1

            line += "\nGO"
            text_file.write(line+"\n")

        firstLine=False

    openFile.close()
    text_file.close()

def csv_to_sql3(baseFile, sqlFile, tableName):

    text_file = open(sqlFile, "w")
    openFile = open(baseFile, 'r')
    csvFile = csv.reader(openFile)
    firstLine=True

    for row in csvFile:

        if(not firstLine):
            #begin the line
            line = "INSERT INTO " + tableName + " VALUES ( "

            #counter
            i=0

            for value in row[::-1]:
                value = value.replace("\"","'")

                #check if is the last value
                if(i == len(row)-1 ):
                    if(isint(value) or isfloat(value)):
                        line += value + " )"
                    else:
                        if not value: 
                            line+= " NULL ) "
                        elif value[0] == "'":
                            line+=value+ " ) "
                        else:
                            line+="'"+value+"'" + " )"

                else:
                    if(isint(value) or isfloat(value)):
                        line += value + ", "
                    else:
                        if not value: 
                            line+= " NULL, "
                        elif value[0] == "'":
                            line+=value+ ", "
                        else:
                            line+="'"+value+"'" + ", "

                i +=1

            line += "\nGO"
            text_file.write(line+"\n")

        firstLine=False

    openFile.close()
    text_file.close()

csv_to_sql("DataSets/Games.csv", "InsertionSQL/GamesSQL.txt", "[Games]")
csv_to_sql("DataSets/Developers.csv", "InsertionSQL/DevelopersSQL.txt", "[Developers]")
csv_to_sql("DataSets/Events.csv", "InsertionSQL/EventsSQL.txt", "[Events]")
csv_to_sql("DataSets/EventType.csv", "InsertionSQL/EventTypeSQL.txt", "[EventType]")
csv_to_sql("DataSets/Franchises.csv", "InsertionSQL/FranchisesSQL.txt", "[Franchises]")
csv_to_sql2("DataSets/GameBelongsFranchise.csv", "InsertionSQL/GameBelongsFranchiseSQL.txt", "[GameBelongsFranchise]")
csv_to_sql2("DataSets/GameDeveloper.csv", "InsertionSQL/GameDeveloperSQL.txt", "[GameDeveloper]")
csv_to_sql2("DataSets/GameEventList.csv", "InsertionSQL/GameEventListSQL.txt", "[GameEventList]")
csv_to_sql3("DataSets/GameGenre.csv", "InsertionSQL/GameGenreSQL.txt", "[GameGenre]")
csv_to_sql("DataSets/Genres.csv", "InsertionSQL/GenresSQL.txt", "[Genres]")
csv_to_sql("DataSets/Platforms.csv", "InsertionSQL/PlatformsSQL.txt", "[Platforms]")
csv_to_sql("DataSets/Publishers.csv", "InsertionSQL/PublishersSQL.txt", "[Publishers]")
csv_to_sql2("DataSets/Releases.csv", "InsertionSQL/ReleasesSQL.txt", "[Releases]")
csv_to_sql("DataSets/Reviews.csv", "InsertionSQL/ReviewsSQL.txt", "[Reviews]")
csv_to_sql("DataSets/Tournments.csv", "InsertionSQL/TournmentsSQL.txt", "[Tournments]")