package com.sdl.smarttarget.extensions.campaigns.replication;

import org.apache.commons.cli.*;

public class Main {

    public static void main(String[] args) {

        try {

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

            CommandLineParser parser = new DefaultParser();
            CommandLine cmdline = parser.parse(options, args);

            String sourceURL = cmdline.getOptionValue("source");
            String targetURL = cmdline.getOptionValue("target");

            System.out.println("Using source instance " + sourceURL);
            System.out.println("Using target instance " + targetURL);

        } catch (ParseException e) {
            e.printStackTrace();
        }


    }

}
