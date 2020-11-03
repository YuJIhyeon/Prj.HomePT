import sklearn as sk
import numpy as np

def get_input_data(filename):
    input_file = open(filename, 'r', encoding='UTF8')
    Excercise=[]

    for line in input_file:
        tmp = []
        line = line.replace('\n','')
        line = line.replace('\t'," ")
        line = line.split(" ")

        #print(line)
        for sig in line:
            tmp.append(float(sig))

        Excercise.append(tmp.copy())

    return Excercise

def TrainData() :

    Data = dict()

    curl = get_input_data("덤벨컬_학습용.txt")
    Data["Curl"] = curl

    kickBack = get_input_data("덤벨킥백_학습용.txt")
    Data["kickBack"] = kickBack

    #for d in Data.keys():
        #print(f"{Data[d]}:{len(Data[d])}")

    return Data

def GenerateDataForm(data):
    pass


def main():
    trainingD = TrainData()
    #print(trainingD)

if __name__ == '__main__':
    main()