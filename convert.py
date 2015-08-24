import csv
import json

data = []
with open("level.txt", 'rb') as f:
    base = None
    reader = csv.reader(f)
    for r in reader:
        if r[0] == "Type":
            base = r
            continue
        obj = {"Position": {}, "Scale": {}}
        for i in range(6):
            if base[i].startswith("Position"):
                obj["Position"][base[i].split(" ")[1]] = r[i]
            elif base[i].startswith("Scale"):
                obj["Scale"][base[i].split(" ")[1]] = r[i]
            elif base[i] == "Type":
                obj["Type"] = r[i]
        data.append(obj)
fp = open("level_1.json", 'wb')
json.dump(data, fp)
fp.close()
        
