import csv
import json

data = {"NORMAL": [], "SHADOWS": [], "SPECIAL": [], "LEVEL DATA": {}}
level_number = raw_input("Level number: ")
next_level = raw_input("Next level: ") 
if "level_" not in level_number:
    level_number = "level_" + level_number
if "level_" not in next_level:
    next_level = "level_" + next_level
data["LEVEL DATA"]["Level Identifier"] = level_number
data["LEVEL DATA"]["Next Level"] = next_level
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
                data[r[i]].append(obj)
        #data.append(obj)
fp = open("level_1_new.json", 'wb')
json.dump(data, fp)
fp.close()
        
