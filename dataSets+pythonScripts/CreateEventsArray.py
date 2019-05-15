import csv

def csv_to_array(baseFile,arrayFile,delimiter):

    text_file = open(arrayFile, "w")
    openFile = open(baseFile, 'r')
    csvFile = csv.reader(openFile, delimiter=delimiter)
    firstLine=True
    line = '{'
    for row in csvFile:
        line += "('" + row[1] + "'," + row[2] + "):" + row[0] + ","
    line += '}'
    text_file.write(line)


    openFile.close()
    text_file.close()

csv_to_array("DataSets/Events.csv","eventsArray.txt",',')