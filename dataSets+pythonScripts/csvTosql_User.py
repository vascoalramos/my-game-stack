import csv


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
    csvFile = csv.reader(openFile, delimiter=";")
    firstLine=True
    line = 'declare @responseMessage nvarchar(250);\n\n'
    # print(line)
    text_file.write(line+"\n")

    for row in csvFile:
        if (firstLine):
            args = row

        else:
            #begin the line
            line = "exec dbo.uspAddUser "

            #counter
            
            for i in range(0,7):

                #check if we passed last value
                if(i == len(row) ):
                    line += '@responseMsg = @responseMessage OUTPUT\nprint @responseMessage'

                else:
                    value = row[i]
                    value = value.replace("'","''")
                    line += '@'+args[i]+' = '
                    if(isint(value) or isfloat(value)):
                        line += value + " , "
                    else:
                        line+="'"+value+"'" + " , "

            # print(line)
            text_file.write(line+"\n")

        firstLine=False

    openFile.close()
    text_file.close()

csv_to_sql("DataSets/Users.csv", "UserSQL.txt", "User")
