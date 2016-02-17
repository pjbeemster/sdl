package com.sdl.smarttarget.extensions.promotions;

/*
 * Copyright (c) 2016 SDL Group.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
 */

import org.apache.commons.cli.*;

public class Replicate {

    public static void main(String[] args) {

        Options options = new Options();
        options.addOption(
                Option.builder("s")
                        .longOpt("source")
                        .required(true)
                        .hasArg(true)
                        .desc("Fredhopper instance base URL to read campaigns from")
                        .build()
        );
        options.addOption(
                Option.builder("t")
                        .longOpt("target")
                        .required(true)
                        .hasArg(true)
                        .desc("Fredhopper instance base URL to copy campaigs to")
                        .build()
        );

        try {
            CommandLineParser parser = new DefaultParser();
            CommandLine cmdline = parser.parse(options, args);

            String sourceURL = cmdline.getOptionValue("source");
            String targetURL = cmdline.getOptionValue("target");

            System.out.println("Using source instance " + sourceURL);
            System.out.println("Using target instance " + targetURL);

        } catch (ParseException e) {
            System.out.println();
            System.out.println(e.getMessage());
            System.out.println();
            new HelpFormatter().printHelp("java -jar replicate-fredhopper-campaigns.jar", options);
        }

    }
}
