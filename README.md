RocketNozzleNormalShock
-------------------------

Cmdline utility that handles calculates the location where a normal shock will appear inside a rocket nozzle.
-------------------------

Includes two modes, Single Condition Simulation and Variable Condition Simulation

###Single Condition

Simulates the properties of a nozzle at a single chamber pressure and ambient pressure condition.  Good for quick checks.

####Inputs:

**Nozzle Area Ratio:** ratio of exit area of nozzle to throat area  
**Ratio of Specific Heats:** gas property of exhaust; > 1 but <= 1.6666666...

**Chamber Pressure (kPa):** internal pressure of the nozzle chamber; equivalent to stagnation pressure of the flow prior to the shock  
**Ambient Pressure (kPa):** pressure outside the nozzle; the root cause of the normal shock in the nozzle

####Outputs:

**Area Ratio where normal shock occurs** is not saved

###Variable Condition

Sweeps across multiple values of chamber pressure and ambient pressure, printing to a file.  Good for comprehensive sweeps of nozzle data

####Inputs:  

**Nozzle Area Ratio:** ratio of exit area of nozzle to throat area  
**Ratio of Specific Heats:** gas property of exhaust; > 1 but <= 1.6666666...

**Chamber Pressure Max (kPa):** max pressure inside the combustion chamber  
**Chamber Pressure Min (kPa):** min pressure; this will vary with throttling  
**Chamber Pressure Steps:** number of chamber pressure test conditions; must be > 2

**Ambient Pressure Max (kPa):** max pressure outside the nozzle
**Ambient Pressure Min (kPa):** min pressure outside the nozzle; this will vary with altitude  
**Ambient Pressure Steps:** number of ambient pressure test conditions; must be > 2

####Outputs:  

**Text file: ** contains area ratio of shock as a function of chamber pressure and ambient pressure  
Ambinet pressure is listed on the left-most column; chamber pressure is listed on the top-most row
