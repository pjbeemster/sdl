#SDL SmartTarget
##High Availability

For SmartTarget High Availability, we are assuming two separate Fredhopper instances in separate data centers. Network Load Balancers (NLBs) distribute traffic between the two data centers. And in case of a complete outage of one data center, the other one takes over all traffic. 
 
There are two data flows that are relevant for high-availability of SmartTarget: published targeted content (DCPs, dynamic component presentations) and configured promotions (on Tridion UI’s Targeting screen). 

- When publishing dynamic content, Tridion CMS sends published content to the Content Delivery Deployer; in turn, the Deployer forwards the target content to SmartTarget/Fredhopper; 
- To configure promotions, the Tridion Targeting UI communicates with the Content Delivery OData web service; the OData web service communicates with Fredhopper’s configuration web service endpoint on the SmartTarget/FH instance.

The main challenge for SmartTarget high-availability comes in on the 2nd data flow: configuring promotions.
Tridion CM maintains a 1-to-many relationship between the CMS and deployers: a Publication Target in Tridion can contain multiple Deployer delivery endpoints However, a Publication Target will only hold a single OData web service URL for targeting, i.e. a one-to-one relationship. Next, that OData web service has a one-to-one relationship with a Fredhopper instance.

*SmartTarget setup with fail-over*
![][st-ha-overview]

High-Availability requires that promotions created on one Fredhopper instance are also available on the other instance. Hence those need to be replicated to the second Fredhopper instance.

###business.xml
Fredhopper stores 

###Publishing configuration changes
To manually publish the FAS configuration, perform the following steps in Fredhopper Business Manager:

- Press "File"
- Press "File" again, this takes you to the publication manager
- Press "Approve"
- Press "Publish"

Publishing provides the relevant configuration to the attached live instances. For replication this is not enough, and additional actions need to be executed as part of the publication. The additional actions are executed as part of the Fredhopper process, using a custom script.
  
####Customer post-publish script
Developing customer post-publish script requires knowledge about FAS, all other systems to be integrated, and the operating system. To configure Fredhopper to use a post-publish script, use deployment-agent-client:

    bin/deployment-agent-client set-option INSTANCE /com/fredhopper/config/VersionControl/@customer-publish-action=FREDHOPPER/INSTANCE/bin/post-publish.sh
    

More info on Fredhopper Learning Center: [Execute additional actions as part of a configuration publication][lc-post-publish-script]

####smarttarget_conf.xml
SmartTarget can automatically approve and publish configuration changes when Promotions are updated in the Tridion Targeting UI. To do so, the smarttarget_config.xml file of the OData Web Service connecting to Fredhopper must have *Approve every change* set to **true**.

    <?xml version="1.0" encoding="UTF-8">
    <configuration>
		...
		<QueryServer>
			...
			<ApproveEveryChange>true</ApproveEveryChange>
		</QueryServer>
		...
    </configuration>

When set, SmartTarget will publish the configuration changes, triggering the customer post-publish script.
###Replicating promotions between instances
![][st-ha-promotions]
####SSH/SCP authentication

See also: [How to use ssh/scp without password on CentOS][scp-auth]
####Configuration reload
Reload the business.xml configuration on the secondary FH instance by calling its Configuration RESTful endpoint:
<http://sdlfh-live-2.company.com:8180/fredhopper/sysadmin/reload-config.jsp?select=business>

## Alternatives
###Fredhopper Campaigns REST API
> pro: better targeted to campaigs: only handles campaign data, not other business config
> 
> con: deletions more complex to handle; more difficult to run from (bash) script

###Fredhopper Captures (capture-export / capture-import)
> pro: recommended way to synchronize FH instances
> 
> con: Query Server offline during import

## To-do's


[//]:# "REFERENCES"

[st-ha-overview]: https://raw.githubusercontent.com/pjbeemster/sdl/master/smarttarget/documentation/SDL-SmartTarget-HighAvailability.png
[st-ha-promotions]: https://raw.githubusercontent.com/pjbeemster/sdl/master/smarttarget/documentation/SDL-SmartTarget-HighAvailability-Promotions.png

[scp-auth]: https://simonljb123.wordpress.com/2013/07/17/how-to-use-sshscp-without-password-on-centos-6-4/

[lc-post-publish-script]: https://www.fredhopper.com/learningcenter/x/zEEbAQ
