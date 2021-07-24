##Question

If we are going to deploy this on production, what do you think is the next
improvement  that you will prioritize next? This can be a feature, a tech debt, or
an architectural  design.

##Answer

I would prioritize improving the data architecture first. The Employee table could use to have more fields, salary especially should not be hard-coded as it may vary per employee.
Employee types could also do with holding values that can be changed without modifying the code, the tax for example. 