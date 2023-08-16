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
