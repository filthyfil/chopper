# this function is derived from the function e^x, and is defined as the Maclaurin series expansion of e^x,
# which is defined as the infinite (limited to the number of iterations in this calculator) sum
# of [x^n / n!]. the number of iterations increases the precision of the estimation but is more 
# computationally expensive to calculate. 

import math

def exp(x, iterations): 
    return sum([
        x**n / math.factorial(n)
        for n in range (0, iterations)
    ])

print("note: please format the input as the x term then the number of iterations seperated by a space. e.g. 1 100 ")
x, iterations = map(int, input("please define the varaible x, as well as the number of iterations to be ran: ").split())
print(f"exp({x}) with {iterations} iterations = {exp(x, iterations)}")