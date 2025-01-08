# Introduction 

	Steps for running load tests.

# Getting Started

1.	Installation process

	The easiest way to begin using JMeter is to first download the latest production release and install it. 
	The release contains all of the files you need to build and run most types of tests, e.g. Web (HTTP/HTTPS), FTP, JDBC, LDAP, Java, JUnit and more.
	Install JMeter 5.2.1 from official webpage https://jmeter.apache.org/ in local folder and add bin directory to system path on Windows.

2.	Software dependencies

	Java 1.8.0_241-b07
	JMeter 5.2.1

# Test

Don't use GUI mode for load testing!, only for Test creation and Test debugging.

- For execute jmeter in development environment:

	1. Execute in PowerShell console.
		
		jmeter -H proxy.andrade.einsanet.es -P 8080 -u username -a password

	2. Open load tests template.

		Click on File-Open on main memu bar and select load test file RequestsAndLimitsPlan.jmx

	3. Change user defined variables.

		Set values for url and applicationPath variables.
	
	4. Run load tests.

		Click on Run-Start on main menu bar.

- For load testing, use CLI Mode (was NON GUI):
	
	1. Go to the load test template directory in command line console, from the root of the git repository.

		cd test\load

	2. Execute load tests

		2.1. With report
	
		jmeter -n -t RequestsAndLimitsPlan.jmx -l C:\tmp\summary.csv -e -o C:\tmp\results -Jurl=dev.domain.es -JapplicationPath=template/

		For view results go to C:\tmp\results directory and open index.html with web browser.

		2.2. Without report

		jmeter -n -t RequestsAndLimitsPlan.jmx -Jurl=dev.domain.es -JapplicationPath=template/