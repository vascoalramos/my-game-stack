import csv

def csv_to_array(baseFile,arrayFile,index,delimiter):

    text_file = open(arrayFile, "w")
    openFile = open(baseFile, 'r')
    csvFile = csv.reader(openFile, delimiter=delimiter)
    firstLine=True
    line = '['
    for row in csvFile:
        line += "'" + row[index] + "',"
    line += ']'
    text_file.write(line)


    openFile.close()
    text_file.close()

# csv_to_array("calendar.csv", "array.txt",1,',')
csv_to_array("DataSets/Users.csv","array.txt",0,';')
