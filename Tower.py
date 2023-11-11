def tower(n, mode = "A"):
    if (n == 1):
        return ["S", "E"]
    else:
        output = ["S", "E"]
        add = tower(n-1)
        back = []
        front = []
        for i in add:
            if (i == "E"):
                back.append("M")
                front.append(i)
            elif (i == "M"):
                back.append("E")
                front.append("S")
            elif (i == "S"):
                back.append(i)
                front.append("M")
        return back + output + front

out = tower(5)
output = ""
for i in range(0, len(out), 2):
    output += out[i] + out[i+1] + " "
print(output)
print(len(out)/2)


"""
Base case T(1) = SE
T(2) = SM¹ SE M¹E

S start rod
M¹ 2nd rod
M² 3rd rod
E last rod

n disks = T(n-2, swap E and M²) + T(2) + T(n-2, swap S and M²)
T(3) = [SM²] SM¹ SE M¹E [M²E]
T(5) = [SE SM¹ SM² M¹M² EM²] SM¹ SE M¹E [M²S M²M¹ M²E M¹E SE]
"""
